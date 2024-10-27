import Foundation

struct Customer: Codable {
    let id: String
    let fullName: String
    let company: String
    let type: Int
    let photoURL: String
}
