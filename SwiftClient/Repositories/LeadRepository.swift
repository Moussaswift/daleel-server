import Foundation

protocol LeadRepositoryProtocol {
    func getLeads(completion: @escaping (Result<[Lead], APIError>) -> Void)
    func getLead(id: String, completion: @escaping (Result<Lead, APIError>) -> Void)
    func createLead(customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void)
    func updateLead(id: String, customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void)
    func deleteLead(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class LeadRepository: LeadRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getLeads(completion: @escaping (Result<[Lead], APIError>) -> Void) {
        apiClient.request(.getLeads, completion: completion)
    }
    
    func getLead(id: String, completion: @escaping (Result<Lead, APIError>) -> Void) {
        apiClient.request(.getLead(id: id), completion: completion)
    }
    
    func createLead(customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void) {
        apiClient.request(.createLead(customerId: customerId, sourceId: sourceId, status: status), completion: completion)
    }
    
    func updateLead(id: String, customerId: String, sourceId: String, status: Int, completion: @escaping (Result<Lead, APIError>) -> Void) {
        apiClient.request(.updateLead(id: id, customerId: customerId, sourceId: sourceId, status: status), completion: completion)
    }
    
    func deleteLead(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deleteLead(id: id), completion: completion)
    }
}