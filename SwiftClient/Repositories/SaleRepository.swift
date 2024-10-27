import Foundation

protocol SaleRepositoryProtocol {
    func getSales(completion: @escaping (Result<[Sale], APIError>) -> Void)
    func getSale(id: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func createSale(date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func updateSale(id: String, date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void)
    func deleteSale(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class SaleRepository: SaleRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getSales(completion: @escaping (Result<[Sale], APIError>) -> Void) {
        apiClient.request(.getSales, completion: completion)
    }
    
    func getSale(id: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        apiClient.request(.getSale(id: id), completion: completion)
    }
    
    func createSale(date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        apiClient.request(.createSale(date: date, customerId: customerId, sourceId: sourceId, itemId: itemId, salesTax: salesTax, discount: discount, total: total, note: note), completion: completion)
    }
    
    func updateSale(id: String, date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String, completion: @escaping (Result<Sale, APIError>) -> Void) {
        apiClient.request(.updateSale(id: id, date: date, customerId: customerId, sourceId: sourceId, itemId: itemId, salesTax: salesTax, discount: discount, total: total, note: note), completion: completion)
    }
    
    func deleteSale(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deleteSale(id: id), completion: completion)
    }
}