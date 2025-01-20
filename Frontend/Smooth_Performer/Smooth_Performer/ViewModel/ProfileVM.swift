//
//  ProfileVM.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-19.
//

import Foundation

@Observable
class ProfileVM:ObservableObject{
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
}
