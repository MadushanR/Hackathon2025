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
        print("Fetching from home view model")
        
        do{
            print("Just before vm function")
            if let currentUser = try fetcher.fetchCurrentStudent(){
                student = fetcher.deleteStudentUserDefault(student: currentUser)
            }
            print("Fetched form signup VM")
            
            status = .success
            print("success From home VM")
            return true
        }catch{
            status = .failed(message: "User not found/Wrong Credentials")
            return false
        }
    }

}

