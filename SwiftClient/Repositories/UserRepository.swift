import Foundation

protocol UserRepositoryProtocol {
    func getUsers(completion: @escaping (Result<[User], APIError>) -> Void)
    func getUser(id: Int, completion: @escaping (Result<User, APIError>) -> Void)
    func createUser(name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void)
    func updateUser(id: Int, name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void)
    func deleteUser(id: Int, completion: @escaping (Result<Void, APIError>) -> Void)
}

class UserRepository: UserRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getUsers(completion: @escaping (Result<[User], APIError>) -> Void) {
        apiClient.request(.getUsers, completion: completion)
    }
    
    func getUser(id: Int, completion: @escaping (Result<User, APIError>) -> Void) {
        apiClient.request(.getUser(id: id), completion: completion)
    }
    
    func createUser(name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void) {
        apiClient.request(.createUser(name: name, email: email), completion: completion)
    }
    
    func updateUser(id: Int, name: String, email: String, completion: @escaping (Result<User, APIError>) -> Void) {
        apiClient.request(.updateUser(id: id, name: name, email: email), completion: completion)
    }
    
    func deleteUser(id: Int, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deleteUser(id: id), completion: completion)
    }
}
