import Foundation

protocol ItemInteractorProtocol {
    func getItems(completion: @escaping (Result<[Item], APIError>) -> Void)
    func getItem(id: String, completion: @escaping (Result<Item, APIError>) -> Void)
    func createItem(name: String, quantity: Int, notes: String, price: Double, completion: @escaping (Result<Item, APIError>) -> Void)
    func updateItem(id: String, name: String, quantity: Int, notes: String, price: Double, completion: @escaping (Result<Item, APIError>) -> Void)
    func deleteItem(id: String, completion: @escaping (Result<Void, APIError>) -> Void)
}

class ItemInteractor: ItemInteractorProtocol {
    private let repository: ItemRepositoryProtocol
    
    init(repository: ItemRepositoryProtocol) {
        self.repository = repository
    }
    
    func getItems(completion: @escaping (Result<[Item], APIError>) -> Void) {
        repository.getItems(completion: completion)
    }
    
    func getItem(id: String, completion: @escaping (Result<Item, APIError>) -> Void) {
        repository.getItem(id: id, completion: completion)
    }
    
    func createItem(name: String, quantity: Int, notes: String, price: Double, completion: @escaping (Result<Item, APIError>) -> Void) {
        repository.createItem(name: name, quantity: quantity, notes: notes, price: price, completion: completion)
    }
    
    func updateItem(id: String, name: String, quantity: Int, notes: String, price: Double, completion: @escaping (Result<Item, APIError>) -> Void) {
        repository.updateItem(id: id, name: name, quantity: quantity, notes: notes, price: price, completion: completion)
    }
    
    func deleteItem(id: String, completion: @escaping (Result<Void, APIError>) -> Void) {
        repository.deleteItem(id: id, completion: completion)
    }
}
