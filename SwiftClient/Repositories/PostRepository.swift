import Foundation

protocol PostRepositoryProtocol {
    func getPosts(completion: @escaping (Result<[Post], APIError>) -> Void)
    func getPost(id: Int, completion: @escaping (Result<Post, APIError>) -> Void)
    func createPost(userId: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void)
    func updatePost(id: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void)
    func deletePost(id: Int, completion: @escaping (Result<Void, APIError>) -> Void)
}

class PostRepository: PostRepositoryProtocol {
    private let apiClient: APIClientProtocol
    
    init(apiClient: APIClientProtocol) {
        self.apiClient = apiClient
    }
    
    func getPosts(completion: @escaping (Result<[Post], APIError>) -> Void) {
        apiClient.request(.getPosts, completion: completion)
    }
    
    func getPost(id: Int, completion: @escaping (Result<Post, APIError>) -> Void) {
        apiClient.request(.getPost(id: id), completion: completion)
    }
    
    func createPost(userId: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void) {
        apiClient.request(.createPost(userId: userId, title: title, body: body), completion: completion)
    }
    
    func updatePost(id: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void) {
        apiClient.request(.updatePost(id: id, title: title, body: body), completion: completion)
    }
    
    func deletePost(id: Int, completion: @escaping (Result<Void, APIError>) -> Void) {
        apiClient.request(.deletePost(id: id), completion: completion)
    }
}
