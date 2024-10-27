import Foundation

struct Lead: Codable {
    let id: String
    let customerId: String
    let sourceId: String
    let status: Int
}
