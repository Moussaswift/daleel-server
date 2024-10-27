import Foundation

protocol UserInteractorProtocol {
    func getUsers(completion: @escaping (Result<[User], APIError>) -> Void)
    func getUser(id: Int, completion: @escaping (Result<User, APIError>) -> Void)
    func createUser(name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void)
    func updateUser(id: Int, name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void)
    func deleteUser(id: Int, completion: @escaping (Result<Void, APIError>) -> Void)
}

class UserInteractor: UserInteractorProtocol {
    private let repository: UserRepositoryProtocol
    
    init(repository: UserRepositoryProtocol) {
        self.repository = repository
    }
    
    func getUsers(completion: @escaping (Result<[User], APIError>) -> Void) {
        repository.getUsers(completion: completion)
    }
    
    func getUser(id: Int, completion: @escaping (Result<User, APIError>) -> Void) {
        repository.getUser(id: id, completion: completion)
    }
    
    func createUser(name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void) {
        repository.createUser(name: name, email: email, completion: completion)
    }
    
    func updateUser(id: Int, name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void) {
        repository.updateUser(id: id, name: name, email: email, completion: completion)
    }
    
    func deleteUser(id: Int, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteUser(id: id, completion: completion)
    }
}
