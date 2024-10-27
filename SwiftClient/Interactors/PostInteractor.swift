import Foundation

protocol PostInteractorProtocol {
    func getPosts(completion: @escaping (Result<[Post], APIError>) -> Void)
    func getPost(id: Int, completion: @escaping (Result<Post, APIError>) -> Void)
    func createPost(userId: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void)
    func updatePost(id: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void)
    func deletePost(id: Int, completion: @escaping (Result<Void, APIError>) -> Void)
}

class PostInteractor: PostInteractorProtocol {
    private let repository: PostRepositoryProtocol
    
    init(repository: PostRepositoryProtocol) {
        self.repository = repository
    }
    
    func getPosts(completion: @escaping (Result<[Post], APIError>) -> Void) {
        repository.getPosts(completion: completion)
    }
    
    func getPost(id: Int, completion: @escaping (Result<Post, APIError>) -> Void) {
        repository.getPost(id: id, completion: completion)
    }
    
    func createPost(userId: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void) {
        repository.createPost(userId: userId, title: title, body: body, completion: completion)
    }
    
    func updatePost(id: Int, title: String, body: String, completion: @escaping (Result<Post, APIError>) -> Void) {
        repository.updatePost(id: id, title: title, body: body, completion: completion)
    }
    
    func deletePost(id: Int, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deletePost(id: id, completion: completion)
    }
}
