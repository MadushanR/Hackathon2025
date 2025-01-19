//
//  User.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

struct Student: Identifiable,Codable{
    let firstName:String
    let lastName:String
    let id:Int
    let email:String
    let password:String
    var courses:[Course]?
    
    enum UserKeys:String,CodingKey{
        case firstName
        case lastName
        case id = "studentId"
        case email
        case password
        case courses
    }
    
    init(from decoder: any Decoder) throws {
        let container = try decoder.container(keyedBy: UserKeys.self)
        self.firstName = try container.decode(String.self, forKey: .firstName)
        self.lastName = try container.decode(String.self, forKey: .lastName)
        self.id = try container.decode(Int.self, forKey: .id)
        self.email = try container.decode(String.self, forKey: .email)
        self.password = try container.decode(String.self, forKey: .password)
        self.courses = try container.decode([Course]?.self, forKey: .courses)
    }
    
    
}
