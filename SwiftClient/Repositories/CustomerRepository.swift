import Foundation

protocol CustomerRepositoryProtocol {
    func getCustomers(completion: @escaping (Result<[Customer], APIError>) -> Void)
    func getCustomer(id: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func createCustomer(fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func updateCustomer(id: String, fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func deleteCustomer(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class CustomerRepository: CustomerRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getCustomers(completion: @escaping (Result<[Customer], APIError>) -> Void) {
        apiClient.request(.getCustomers, completion: completion)
    }
    
    func getCustomer(id: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        apiClient.request(.getCustomer(id: id), completion: completion)
    }
    
    func createCustomer(fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        apiClient.request(.createCustomer(fullName: fullName, company: company, type: type, photoURL: photoURL), completion: completion)
    }
    
    func updateCustomer(id: String, fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        apiClient.request(.updateCustomer(id: id, fullName: fullName, company: company, type: type, photoURL: photoURL), completion: completion)
    }
    
    func deleteCustomer(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deleteCustomer(id: id), completion: completion)
    }
}
