import Foundation

protocol SaleInteractorProtocol {
    func getSales(completion: @escaping (Result<[Sale], APIError>) -> Void)
    func getSale(id: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func createSale(date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func updateSale(id: String, date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func deleteSale(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class SaleInteractor: SaleInteractorProtocol {
    private let repository: SaleRepositoryProtocol
    
    init(repository: SaleRepositoryProtocol) {
        self.repository = repository
    }
    
    func getSales(completion: @escaping (Result<[Sale], APIError>) -> Void) {
        repository.getSales(completion: completion)
    }
    
    func getSale(id: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        repository.getSale(id: id, completion: completion)
    }
    
    func createSale(date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        repository.createSale(date: date, customerId: customerId, sourceId: sourceId, itemId: itemId, salesTax: salesTax, discount: discount, total: total, note: note, completion: completion)
    }
    
    func updateSale(id: String, date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        repository.updateSale(id: id, date: date, customerId: customerId, sourceId: sourceId, itemId: itemId, salesTax: salesTax, discount: discount, total: total, note: note, completion: completion)
    }
    
    func deleteSale(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteSale(id: id, completion: completion)
    }
}