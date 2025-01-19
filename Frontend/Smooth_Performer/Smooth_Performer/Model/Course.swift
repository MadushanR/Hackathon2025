//
//  Courses.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

struct Course:Identifiable,Codable{
    var id:Int
    var courseName:String
    var sectionId:Int
    var grade:Double
    var credits:Int
    
    enum CourseKeys:String,CodingKey{
        case id = "courseId"
        case courseName
        case sectionId
        case grade
        case credits
    }
    
    init(from decoder: any Decoder) throws {
        let container = try decoder.container(keyedBy: CourseKeys.self)
        self.id = try container.decode(Int.self, forKey: .id)
        self.courseName = try container.decode(String.self, forKey: .courseName)
        self.sectionId = try container.decode(Int.self, forKey: .sectionId)
        self.grade = try container.decode(Double.self, forKey: .grade)
        self.credits = try container.decode(Int.self, forKey: .credits)
    }
    
}
