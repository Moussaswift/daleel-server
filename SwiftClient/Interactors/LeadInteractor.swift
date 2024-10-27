import Foundation

protocol LeadInteractorProtocol {
    func getLeads(completion: @escaping (Result<[Lead], APIError>) -> Void)
    func getLead(id: String, completion: @escaping (Result<Lead, APIError>) -> Void)
    func createLead(customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void)
    func updateLead(id: String, customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void)
    func deleteLead(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class LeadInteractor: LeadInteractorProtocol {
    private let repository: LeadRepositoryProtocol
    
    init(repository: LeadRepositoryProtocol) {
        self.repository = repository
    }
    
    func getLeads(completion: @escaping (Result<[Lead], APIError>) -> Void) {
        repository.getLeads(completion: completion)
    }
    
    func getLead(id: String, completion: @escaping (Result<Lead, APIError>) -> Void) {
        repository.getLead(id: id, completion: completion)
    }
    
    func createLead(customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void) {
        repository.createLead(customerId: customerId, sourceId: sourceId, status: status, completion: completion)
    }
    
    func updateLead(id: String, customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void) {
        repository.updateLead(id: id, customerId: customerId, sourceId: sourceId, status: status, completion: completion)
    }
    
    func deleteLead(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteLead(id: id, completion: completion)
    }
}