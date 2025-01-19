//
//  HomeVM.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-19.
//

import Foundation

//deleteStudentUserDefault

@Observable
class HomeVM:ObservableObject{
    enum FetchStatus{
        case notStarted
        case fetching
        case success
        case failed(message: String)
    }
    
    private(set) var status:FetchStatus = .notStarted
    
    private let fetcher = FetchService()
    
    var student: Student? = nil
    
    func signOut() -> Bool{
        status = .fetching
        
        do{
            if let currentUser = try fetcher.fetchCurrentStudent(){
                student = fetcher.deleteStudentUserDefault(student: currentUser)
            }
            
            status = .success
            return true
        }catch{
            status = .failed(message: "User not found/Wrong Credentials")
            return false
        }
    }
    
    func addCourses(id:Int) async -> [Course]{
        var course:[Course]
        status = .fetching
        print("Fetching from home view model course")
        
        do{
            print("Just before vm function course")
            course = try await fetcher.fetchCourse(for: id)
            print("Fetched form home VM course")
            
            status = .success
            print("success From home VM course")
            return course
        }catch{
            status = .failed(message: "User not found/Wrong Credentials")
            return []
        }
        
    }

}

