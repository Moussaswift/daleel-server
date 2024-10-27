import Foundation

struct Sale: Codable {
    let id: String
    let date: Date
    let customerId: String
    let sourceId: String
    let itemId: String
    let salesTax: Double
    let discount: Double
    let total: Double
    let note: String
}
