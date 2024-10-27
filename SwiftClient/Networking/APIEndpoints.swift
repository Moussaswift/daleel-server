import Alamofire

enum APIEndpoint {
    // Customers
    case getCustomers
    case getCustomer(id: String)
    case createCustomer(fullName: String, company: String, type: Int, photoURL: String)
    case updateCustomer(id: String, fullName: String, company: String, type: Int, photoURL: String)
    case deleteCustomer(id: String)
    
    // Items
    case getItems
    case getItem(id: String)
    case createItem(name: String, quantity: Int, notes: String, price: Double)
    case updateItem(id: String, name: String, quantity: Int, notes: String, price: Double)
    case deleteItem(id: String)
    
    // Leads
    case getLeads
    case getLead(id: String)
    case createLead(customerId: String, sourceId: String, status: Int)
    case updateLead(id: String, customerId: String, sourceId: String, status: Int)
    case deleteLead(id: String)
    
    // Sales
    case getSales
    case getSale(id: String)
    case createSale(date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String)
    case updateSale(id: String, date: Date, customerId: String, sourceId: String, itemId: String, salesTax: Double, discount: Double, total: Double, note: String)
    case deleteSale(id: String)
    
    // Sources
    case getSources
    case getSource(id: String)
    case createSource(name: String)
    case updateSource(id: String, name: String)
    case deleteSource(id: String)
    
    var path: String {
        switch self {
        case .getCustomers, .createCustomer: return "api/Customers"
        case .getCustomer(let id), .updateCustomer(let id, _, _, _, _), .deleteCustomer(let id): return "api/Customers/\(id)"
        case .getItems, .createItem: return "api/Items"
        case .getItem(let id), .updateItem(let id, _, _, _, _), .deleteItem(let id): return "api/Items/\(id)"
        case .getLeads, .createLead: return "api/Leads"
        case .getLead(let id), .updateLead(let id, _, _, _), .deleteLead(let id): return "api/Leads/\(id)"
        case .getSales, .createSale: return "api/Sales"
        case .getSale(let id), .updateSale(let id, _, _, _, _, _, _, _, _), .deleteSale(let id): return "api/Sales/\(id)"
        case .getSources, .createSource: return "api/Sources"
        case .getSource(let id), .updateSource(let id, _), .deleteSource(let id): return "api/Sources/\(id)"
        }
    }
    
    var method: HTTPMethod {
        switch self {
        case .getCustomers, .getCustomer, .getItems, .getItem, .getLeads, .getLead, .getSales, .getSale, .getSources, .getSource:
            return .get
        case .createCustomer, .createItem, .createLead, .createSale, .createSource:
            return .post
        case .updateCustomer, .updateItem, .updateLead, .updateSale, .updateSource:
            return .put
        case .deleteCustomer, .deleteItem, .deleteLead, .deleteSale, .deleteSource:
            return .delete
        }
    }
    
    var parameters: Parameters? {
        switch self {
        case .createCustomer(let fullName, let company, let type, let photoURL), .updateCustomer(_, let fullName, let company, let type, let photoURL):
            return ["fullName": fullName, "company": company, "type": type, "photoURL": photoURL]
        case .createItem(let name, let quantity, let notes, let price), .updateItem(_, let name, let quantity, let notes, let price):
            return ["name": name, "quantity": quantity, "notes": notes, "price": price]
        case .createLead(let customerId, let sourceId, let status), .updateLead(_, let customerId, let sourceId, let status):
            return ["customerId": customerId, "sourceId": sourceId, "status": status]
        case .createSale(let date, let customerId, let sourceId, let itemId, let salesTax, let discount, let total, let note),
             .updateSale(_, let date, let customerId, let sourceId, let itemId, let salesTax, let discount, let total, let note):
            return ["date": date, "customerId": customerId, "sourceId": sourceId, "itemId": itemId, "salesTax": salesTax, "discount": discount, "total": total, "note": note]
        case .createSource(let name), .updateSource(_, let name):
            return ["name": name]
        default:
            return nil
        }
    }
    
    var encoding: ParameterEncoding {
        switch method {
        case .get:
            return URLEncoding.default
        default:
            return JSONEncoding.default
        }
    }
    
    var headers: HTTPHeaders? {
        return ["Content-Type": "application/json"]
    }
}
