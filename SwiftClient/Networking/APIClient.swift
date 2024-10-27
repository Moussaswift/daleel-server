import Foundation
import Alamofire

protocol APIClientProtocol {
    func request<T: Decodable>(_ endpoint: APIEndpoint, completion: @escaping (Result<T, APIError>) -> Void)
}

class APIClient: APIClientProtocol {
    private let baseURL: URL
    
    init(baseURL: URL) {
        self.baseURL = baseURL
    }
    
    func request<T: Decodable>(_ endpoint: APIEndpoint, completion: @escaping (Result<T, APIError>) -> Void) {
        let url = baseURL.appendingPathComponent(endpoint.path)
        
        AF.request(url, method: endpoint.method, parameters: endpoint.parameters, encoding: endpoint.encoding, headers: endpoint.headers)
            .validate()
            .responseDecodable(of: T.self) { response in
                switch response.result {
                case .success(let value):
                    completion(.success(value))
                case .failure(let error):
                    completion(.failure(APIError.networkError(error)))
                }
            }
    }
}
