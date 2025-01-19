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
    
    enum StudentJson:String,CodingKey{
        case student
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
    }
    
    init(from decoder: any Decoder) throws {
        let container = try decoder.container(keyedBy: StudentJson.self)
        let studentContainer = try container.nestedContainer(keyedBy: StudentJson.StudentKeys.self, forKey: .student)
        
        self.firstName = try studentContainer.decode(String.self, forKey: .firstName)
        self.lastName = try studentContainer.decode(String.self, forKey: .lastName)
        self.id = try studentContainer.decode(Int.self, forKey: .id)
        self.email = try studentContainer.decode(String.self, forKey: .email)
        self.password = try studentContainer.decode(String.self, forKey: .password)
        self.gpa = try studentContainer.decode(Int.self, forKey: .gpa)
        self.dGpa = try studentContainer.decode(Int.self, forKey: .dGpa)
        self.courses = try studentContainer.decodeIfPresent([Course].self, forKey: .courses)
    }
    
    
}
