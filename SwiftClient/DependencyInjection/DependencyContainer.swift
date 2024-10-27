import Foundation

class DependencyContainer {
    static let shared = DependencyContainer()
    
    private init() {}
    
    lazy var apiClient: APIClientProtocol = {
        let baseURL = URL(string: "https://jsonplaceholder.typicode.com")!
        return APIClient(baseURL: baseURL)
    }()
    
    lazy var userRepository: UserRepositoryProtocol = {
        return UserRepository(apiClient: apiClient)
    }()
    
    lazy var postRepository: PostRepositoryProtocol = {
        return PostRepository(apiClient: apiClient)
    }()
    
    lazy var userInteractor: UserInteractorProtocol = {
        return UserInteractor(repository: userRepository)
    }()
    
    lazy var postInteractor: PostInteractorProtocol = {
        return PostInteractor(repository: postRepository)
    }()
}
