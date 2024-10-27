import CoreData

class ItemRepository: ItemRepositoryProtocol {
    private let apiClient: APIClientProtocol
    private let context: NSManagedObjectContext
    
    init(apiClient: APIClientProtocol, context: NSManagedObjectContext = CoreDataStack.shared.context) {
        self.apiClient = apiClient
        self.context = context
    }
    
    func getItems(completion: @escaping (Result<[Item], APIError>) -> Void) {
        let fetchRequest: NSFetchRequest<ItemEntity> = ItemEntity.fetchRequest()
        
        do {
            let itemEntities = try context.fetch(fetchRequest)
            let items = itemEntities.map { Item(from: $0) }
            completion(.success(items))
        } catch {
            completion(.failure(.decodingError(error)))
        }
    }
    
    func createItem(name: String, quantity: Int, notes: String, price: Double, completion: @escaping (Result<Item, APIError>) -> Void) {
        let itemEntity = ItemEntity(context: context)
        itemEntity.id = UUID().uuidString
        itemEntity.name = name
        itemEntity.quantity = Int32(quantity)
        itemEntity.notes = notes
        itemEntity.price = price
        itemEntity.lastModified = Date()
        
        do {
            try context.save()
            let item = Item(from: itemEntity)
            completion(.success(item))
            
            // Sync with backend
            syncItem(item)
        } catch {
            completion(.failure(.decodingError(error)))
        }
    }
    
    private func syncItem(_ item: Item) {
        apiClient.request(.createItem(name: item.name, quantity: item.quantity, notes: item.notes, price: item.price)) { (result: Result<Item, APIError>) in
            switch result {
            case .success(let syncedItem):
                // Update local item with synced data if needed
                self.updateLocalItem(syncedItem)
            case .failure(let error):
                print("Failed to sync item: \(error)")
            }
        }
    }
    
    private func updateLocalItem(_ item: Item) {
        let fetchRequest: NSFetchRequest<ItemEntity> = ItemEntity.fetchRequest()
        fetchRequest.predicate = NSPredicate(format: "id == %@", item.id)
        
        do {
            let results = try context.fetch(fetchRequest)
            if let existingItem = results.first {
                existingItem.name = item.name
                existingItem.quantity = Int32(item.quantity)
                existingItem.notes = item.notes
                existingItem.price = item.price
                existingItem.lastModified = Date()
                
                try context.save()
            }
        } catch {
            print("Failed to update local item: \(error)")
        }
    }
    
    // Implement other CRUD operations similarly
}
