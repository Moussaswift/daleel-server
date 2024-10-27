import Foundation

protocol CustomerInteractorProtocol {
    func getCustomers(completion: @escaping (Result<[Customer], APIError>) -> Void)
    func getCustomer(id: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func createCustomer(fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func updateCustomer(id: String, fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void)
    func deleteCustomer(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class CustomerInteractor: CustomerInteractorProtocol {
    private let repository: CustomerRepositoryProtocol
    
    init(repository: CustomerRepositoryProtocol) {
        self.repository = repository
    }
    
    func getCustomers(completion: @escaping (Result<[Customer], APIError>) -> Void) {
        repository.getCustomers(completion: completion)
    }
    
    func getCustomer(id: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        repository.getCustomer(id: id, completion: completion)
    }
    
    func createCustomer(fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        repository.createCustomer(fullName: fullName, company: company, type: type, photoURL: photoURL, completion: completion)
    }
    
    func updateCustomer(id: String, fullName: String, company: String, type: Int, photoURL: String, completion: @escaping (Result<Customer, APIError>) -> Void) {
        repository.updateCustomer(id: id, fullName: fullName, company: company, type: type, photoURL: photoURL, completion: completion)
    }
    
    func deleteCustomer(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteCustomer(id: id, completion: completion)
    }
}
