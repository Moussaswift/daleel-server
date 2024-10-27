import Foundation

protocol SourceInteractorProtocol {
    func getSources(completion: @escaping (Result<[Source], APIError>) -> Void)
    func getSource(id: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func createSource(name: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func updateSource(id: String, name: String, completion: @escaping (Result<Source, APIError>) -> Void)
    func deleteSource(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class SourceInteractor: SourceInteractorProtocol {
    private let repository: SourceRepositoryProtocol
    
    init(repository: SourceRepositoryProtocol) {
        self.repository = repository
    }
    
    func getSources(completion: @escaping (Result<[Source], APIError>) -> Void) {
        repository.getSources(completion: completion)
    }
    
    func getSource(id: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        repository.getSource(id: id, completion: completion)
    }
    
    func createSource(name: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        repository.createSource(name: name, completion: completion)
    }
    
    func updateSource(id: String, name: String, completion: @escaping (Result<Source, APIError>) -> Void) {
        repository.updateSource(id: id, name: name, completion: completion)
    }
    
    func deleteSource(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteSource(id: id, completion: completion)
    }
}