import Foundation

enum APIError: Error {
    case networkError(Error)
    case decodingError(Error)
    case invalidResponse
    case unauthorized
    case notFound
    case serverError
    
    var localizedDescription: String {
        switch self {
        case .networkError(let error):
            return "Network error: \(error.localizedDescription)"
        case .decodingError(let error):
            return "Decoding error: \(error.localizedDescription)"
        case .invalidResponse:
            return "Invalid response from the server"
        case .unauthorized:
            return "Unauthorized access"
        case .notFound:
            return "Resource not found"
        case .serverError:
            return "Server error"
        }
    }
}
