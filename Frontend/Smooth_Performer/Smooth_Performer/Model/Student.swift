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
    let gpa:Int
    let dGpa:Int
    var courses:[Course]?
    
    enum StudentKeys:String,CodingKey{
        case firstName
        case lastName
        case id = "studentId"
        case email
        case password
        case gpa
        case dGpa = "desiredGPA"
        case courses
    }
    
    init(from decoder: any Decoder) throws {
        let container = try decoder.container(keyedBy: StudentKeys.self)
        self.firstName = try container.decode(String.self, forKey: .firstName)
        self.lastName = try container.decode(String.self, forKey: .lastName)
        self.id = try container.decode(Int.self, forKey: .id)
        self.email = try container.decode(String.self, forKey: .email)
        self.password = try container.decode(String.self, forKey: .password)
        self.gpa = try container.decode(Int.self, forKey: .gpa)
        self.dGpa = try container.decode(Int.self, forKey: .dGpa)
        self.courses = try container.decodeIfPresent([Course].self, forKey: .courses)
    }
    
    
}
