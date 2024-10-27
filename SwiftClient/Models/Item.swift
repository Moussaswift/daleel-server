import Foundation

struct Item: Codable {
    let id: String
    let name: String
    let quantity: Int
    let notes: String
    let price: Double
    let lastModified: Date
    
    init(from entity: ItemEntity) {
        self.id = entity.id ?? UUID().uuidString
        self.name = entity.name ?? ""
        self.quantity = Int(entity.quantity)
        self.notes = entity.notes ?? ""
        self.price = entity.price
        self.lastModified = entity.lastModified ?? Date()
    }
}
