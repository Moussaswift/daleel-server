import Foundation
import CoreData

class SyncService {
    private let apiClient: APIClientProtocol
    private let context: NSManagedObjectContext
    
    init(apiClient: APIClientProtocol, context: NSManagedObjectContext = CoreDataStack.shared.context) {
        self.apiClient = apiClient
        self.context = context
    }
    
    func syncAll(completion: @escaping (Error?) -> Void) {
        let group = DispatchGroup()
        var syncError: Error?
        
        group.enter()
        syncCustomers { error in
            if let error = error { syncError = error }
            group.leave()
        }
        
        group.enter()
        syncItems { error in
            if let error = error { syncError = error }
            group.leave()
        }
        
        group.enter()
        syncLeads { error in
            if let error = error { syncError = error }
            group.leave()
        }
        
        group.enter()
        syncSales { error in
            if let error = error { syncError = error }
            group.leave()
        }
        
        group.enter()
        syncSources { error in
            if let error = error { syncError = error }
            group.leave()
        }
        
        group.notify(queue: .main) {
            completion(syncError)
        }
    }
    
    // MARK: - Customers
    
    private func syncCustomers(completion: @escaping (Error?) -> Void) {
        apiClient.request(.getCustomers) { (result: Result<[Customer], APIError>) in
            switch result {
            case .success(let remoteCustomers):
                self.reconcileCustomers(remoteCustomers: remoteCustomers, completion: completion)
            case .failure(let error):
                completion(error)
            }
        }
    }
    
    private func reconcileCustomers(remoteCustomers: [Customer], completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<CustomerEntity> = CustomerEntity.fetchRequest()
        
        do {
            let localCustomers = try context.fetch(fetchRequest)
            
            for remoteCustomer in remoteCustomers {
                if let localCustomer = localCustomers.first(where: { $0.id == remoteCustomer.id }) {
                    updateLocalCustomer(localCustomer, with: remoteCustomer)
                } else {
                    createLocalCustomer(from: remoteCustomer)
                }
            }
            
            for localCustomer in localCustomers {
                if !remoteCustomers.contains(where: { $0.id == localCustomer.id }) {
                    context.delete(localCustomer)
                }
            }
            
            try context.save()
            completion(nil)
        } catch {
            completion(error)
        }
    }
    
    private func updateLocalCustomer(_ localCustomer: CustomerEntity, with remoteCustomer: Customer) {
        localCustomer.fullName = remoteCustomer.fullName
        localCustomer.company = remoteCustomer.company
        localCustomer.type = Int32(remoteCustomer.type)
        localCustomer.photoURL = remoteCustomer.photoURL
    }
    
    private func createLocalCustomer(from remoteCustomer: Customer) {
        let newCustomer = CustomerEntity(context: context)
        newCustomer.id = remoteCustomer.id
        newCustomer.fullName = remoteCustomer.fullName
        newCustomer.company = remoteCustomer.company
        newCustomer.type = Int32(remoteCustomer.type)
        newCustomer.photoURL = remoteCustomer.photoURL
    }
    
    // MARK: - Items
    
    private func syncItems(completion: @escaping (Error?) -> Void) {
        let group = DispatchGroup()
        var syncError: Error?
        
        group.enter()
        pushLocalItems { error in
            if let error = error {
                syncError = error
            }
            group.leave()
        }
        
        group.enter()
        pullRemoteItems { error in
            if let error = error {
                syncError = error
            }
            group.leave()
        }
        
        group.notify(queue: .main) {
            completion(syncError)
        }
    }
    
    private func pushLocalItems(completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<ItemEntity> = ItemEntity.fetchRequest()
        fetchRequest.predicate = NSPredicate(format: "needsSync == YES")
        
        do {
            let localItems = try context.fetch(fetchRequest)
            let group = DispatchGroup()
            var pushError: Error?
            
            for localItem in localItems {
                group.enter()
                pushItemToServer(localItem) { error in
                    if let error = error {
                        pushError = error
                    }
                    group.leave()
                }
            }
            
            group.notify(queue: .main) {
                completion(pushError)
            }
        } catch {
            completion(error)
        }
    }
    
    private func pushItemToServer(_ item: ItemEntity, completion: @escaping (Error?) -> Void) {
        let itemData = Item(from: item)
        
        if item.id.isEmpty {
            // Create new item on server
            apiClient.request(.createItem(name: itemData.name, quantity: itemData.quantity, notes: itemData.notes, price: itemData.price)) { (result: Result<Item, APIError>) in
                switch result {
                case .success(let createdItem):
                    item.id = createdItem.id
                    item.needsSync = false
                    try? self.context.save()
                    completion(nil)
                case .failure(let error):
                    completion(error)
                }
            }
        } else {
            // Update existing item on server
            apiClient.request(.updateItem(id: itemData.id, name: itemData.name, quantity: itemData.quantity, notes: itemData.notes, price: itemData.price)) { (result: Result<Item, APIError>) in
                switch result {
                case .success(_):
                    item.needsSync = false
                    try? self.context.save()
                    completion(nil)
                case .failure(let error):
                    completion(error)
                }
            }
        }
    }
    
    private func pullRemoteItems(completion: @escaping (Error?) -> Void) {
        apiClient.request(.getItems) { (result: Result<[Item], APIError>) in
            switch result {
            case .success(let remoteItems):
                self.reconcileItems(remoteItems: remoteItems, completion: completion)
            case .failure(let error):
                completion(error)
            }
        }
    }
    
    private func reconcileItems(remoteItems: [Item], completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<ItemEntity> = ItemEntity.fetchRequest()
        
        do {
            let localItems = try context.fetch(fetchRequest)
            
            for remoteItem in remoteItems {
                if let localItem = localItems.first(where: { $0.id == remoteItem.id }) {
                    if remoteItem.lastModified > localItem.lastModified {
                        updateLocalItem(localItem, with: remoteItem)
                    }
                } else {
                    createLocalItem(from: remoteItem)
                }
            }
            
            // Don't delete local items not present on the server, they might be new local creations
            
            try context.save()
            completion(nil)
        } catch {
            completion(error)
        }
    }
    
    private func updateLocalItem(_ localItem: ItemEntity, with remoteItem: Item) {
        if !localItem.needsSync {
            localItem.name = remoteItem.name
            localItem.quantity = Int32(remoteItem.quantity)
            localItem.notes = remoteItem.notes
            localItem.price = remoteItem.price
            localItem.lastModified = remoteItem.lastModified
        }
    }
    
    private func createLocalItem(from remoteItem: Item) {
        let newItem = ItemEntity(context: context)
        newItem.id = remoteItem.id
        newItem.name = remoteItem.name
        newItem.quantity = Int32(remoteItem.quantity)
        newItem.notes = remoteItem.notes
        newItem.price = remoteItem.price
        newItem.lastModified = remoteItem.lastModified
        newItem.needsSync = false
    }
    
    // MARK: - Leads
    
    private func syncLeads(completion: @escaping (Error?) -> Void) {
        apiClient.request(.getLeads) { (result: Result<[Lead], APIError>) in
            switch result {
            case .success(let remoteLeads):
                self.reconcileLeads(remoteLeads: remoteLeads, completion: completion)
            case .failure(let error):
                completion(error)
            }
        }
    }
    
    private func reconcileLeads(remoteLeads: [Lead], completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<LeadEntity> = LeadEntity.fetchRequest()
        
        do {
            let localLeads = try context.fetch(fetchRequest)
            
            for remoteLead in remoteLeads {
                if let localLead = localLeads.first(where: { $0.id == remoteLead.id }) {
                    updateLocalLead(localLead, with: remoteLead)
                } else {
                    createLocalLead(from: remoteLead)
                }
            }
            
            for localLead in localLeads {
                if !remoteLeads.contains(where: { $0.id == localLead.id }) {
                    context.delete(localLead)
                }
            }
            
            try context.save()
            completion(nil)
        } catch {
            completion(error)
        }
    }
    
    private func updateLocalLead(_ localLead: LeadEntity, with remoteLead: Lead) {
        localLead.customerId = remoteLead.customerId
        localLead.sourceId = remoteLead.sourceId
        localLead.status = Int32(remoteLead.status)
    }
    
    private func createLocalLead(from remoteLead: Lead) {
        let newLead = LeadEntity(context: context)
        newLead.id = remoteLead.id
        newLead.customerId = remoteLead.customerId
        newLead.sourceId = remoteLead.sourceId
        newLead.status = Int32(remoteLead.status)
    }
    
    // MARK: - Sales
    
    private func syncSales(completion: @escaping (Error?) -> Void) {
        apiClient.request(.getSales) { (result: Result<[Sale], APIError>) in
            switch result {
            case .success(let remoteSales):
                self.reconcileSales(remoteSales: remoteSales, completion: completion)
            case .failure(let error):
                completion(error)
            }
        }
    }
    
    private func reconcileSales(remoteSales: [Sale], completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<SaleEntity> = SaleEntity.fetchRequest()
        
        do {
            let localSales = try context.fetch(fetchRequest)
            
            for remoteSale in remoteSales {
                if let localSale = localSales.first(where: { $0.id == remoteSale.id }) {
                    updateLocalSale(localSale, with: remoteSale)
                } else {
                    createLocalSale(from: remoteSale)
                }
            }
            
            for localSale in localSales {
                if !remoteSales.contains(where: { $0.id == localSale.id }) {
                    context.delete(localSale)
                }
            }
            
            try context.save()
            completion(nil)
        } catch {
            completion(error)
        }
    }
    
    private func updateLocalSale(_ localSale: SaleEntity, with remoteSale: Sale) {
        localSale.date = remoteSale.date
        localSale.customerId = remoteSale.customerId
        localSale.sourceId = remoteSale.sourceId
        localSale.itemId = remoteSale.itemId
        localSale.salesTax = remoteSale.salesTax
        localSale.discount = remoteSale.discount
        localSale.total = remoteSale.total
        localSale.note = remoteSale.note
    }
    
    private func createLocalSale(from remoteSale: Sale) {
        let newSale = SaleEntity(context: context)
        newSale.id = remoteSale.id
        newSale.date = remoteSale.date
        newSale.customerId = remoteSale.customerId
        newSale.sourceId = remoteSale.sourceId
        newSale.itemId = remoteSale.itemId
        newSale.salesTax = remoteSale.salesTax
        newSale.discount = remoteSale.discount
        newSale.total = remoteSale.total
        newSale.note = remoteSale.note
    }
    
    // MARK: - Sources
    
    private func syncSources(completion: @escaping (Error?) -> Void) {
        apiClient.request(.getSources) { (result: Result<[Source], APIError>) in
            switch result {
            case .success(let remoteSources):
                self.reconcileSources(remoteSources: remoteSources, completion: completion)
            case .failure(let error):
                completion(error)
            }
        }
    }
    
    private func reconcileSources(remoteSources: [Source], completion: @escaping (Error?) -> Void) {
        let fetchRequest: NSFetchRequest<SourceEntity> = SourceEntity.fetchRequest()
        
        do {
            let localSources = try context.fetch(fetchRequest)
            
            for remoteSource in remoteSources {
                if let localSource = localSources.first(where: { $0.id == remoteSource.id }) {
                    updateLocalSource(localSource, with: remoteSource)
                } else {
                    createLocalSource(from: remoteSource)
                }
            }
            
            for localSource in localSources {
                if !remoteSources.contains(where: { $0.id == localSource.id }) {
                    context.delete(localSource)
                }
            }
            
            try context.save()
            completion(nil)
        } catch {
            completion(error)
        }
    }
    
    private func updateLocalSource(_ localSource: SourceEntity, with remoteSource: Source) {
        localSource.name = remoteSource.name
    }
    
    private func createLocalSource(from remoteSource: Source) {
        let newSource = SourceEntity(context: context)
        newSource.id = remoteSource.id
        newSource.name = remoteSource.name
    }
}
