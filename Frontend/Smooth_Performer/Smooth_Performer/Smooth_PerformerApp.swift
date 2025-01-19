//
//  Smooth_PerformerApp.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

@main
struct Smooth_PerformerApp: App {
    @State var IsUser = false
    var body: some Scene {
        WindowGroup {
            ZStack{
                NavigationStack{
                    HomeTab()
                }
            }
            .onAppear{
                
            }
            .fullScreenCover(isPresented: $IsUser) {
                NavigationStack{
                    LSTab()
                }
            }
        }
    }
}
