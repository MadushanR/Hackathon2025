//
//  MainViewModel.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

@Observable
final class MainViewModel{
    enum FetchStatus{
        case notStarted
        case fetching
        case success
        case failed(message: String)
    }
    
    private(set) var status:FetchStatus = .notStarted
    
    private let fetcher = FetchService()
    
    var student: Student? = nil
    
    func getCurrentUser() throws ->Student? {
        status = .fetching
        print("Still fetching in main")
        
        do{
            student = try fetcher.fetchCurrentStudent()
            print("Fetched from main")
            
            print(student)
            status = .success
            print("success from main")
        }catch{
            status = .failed(message: "somthing went wrong!")
        }
        
        return student
    }
}
