//
//  User.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

struct Student: Codable{
    let firstName:String
    let lastName:String
    let studentId:Int
    let email:String
    let password:String
    let gpa:Int
    let desiredGPA:Int
    var courses:[Course]?
    
//    var id = {Student.self}
    
    
    
    enum StudentKeys:String,CodingKey{
        case firstName
        case lastName
        case studentId
        case email
        case password
        case gpa
        case desiredGPA
        case courses
    }
    
    init(from decoder: any Decoder) throws {
        let studentContainer = try decoder.container(keyedBy: StudentKeys.self)
        
        self.firstName = try studentContainer.decode(String.self, forKey: .firstName)
        self.lastName = try studentContainer.decode(String.self, forKey: .lastName)
        self.desiredGPA = try studentContainer.decode(Int.self, forKey: .desiredGPA)
        self.studentId = try studentContainer.decode(Int.self, forKey: .studentId)
        self.email = try studentContainer.decode(String.self, forKey: .email)
        self.password = try studentContainer.decode(String.self, forKey: .password)
        self.gpa = try studentContainer.decode(Int.self, forKey: .gpa)
        self.courses = try studentContainer.decodeIfPresent([Course].self, forKey: .courses)
    }
    
    
}
