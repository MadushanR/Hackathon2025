//
//  MainViewModel.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

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
        print("Still fetching")
        
        do{
            student = try fetcher.fetchCurrentStudent()
            
            print("Fetched")
            
            status = .success
            print("success")
        }catch{
            status = .failed(message: "somthing went wrong!")
        }
        
        return student
    }
}
