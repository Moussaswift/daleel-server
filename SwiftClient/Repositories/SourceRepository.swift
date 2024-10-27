import Foundation

protocol SourceRepositoryProtocol {
    func getSources(completion: @escaping (Result<[Source], APIError>) -> Void)
    func getSource(id: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func createSource(name: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func updateSource(id: String, name: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func deleteSource(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class SourceRepository: SourceRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getSources(completion: @escaping (Result<[Source], APIError>) -> Void) {
        apiClient.request(.getSources, completion: completion)
    }
    
    func getSource(id: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        apiClient.request(.getSource(id: id), completion: completion)
    }
    
    func createSource(name: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        apiClient.request(.createSource(name: name), completion: completion)
    }
    
    func updateSource(id: String, name: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        apiClient.request(.updateSource(id: id, name: name), completion: completion)
    }
    
    func deleteSource(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deleteSource(id: id), completion: completion)
    }
}