//
//  HomeTab.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct HomeTab: View {
    @State var currentStudent:Student? = nil
    @Binding var showSLView:Bool
    var body: some View {
        TabView {
            Home(showSLView: $showSLView, student: currentStudent ?? FetchService().student!)
                .tabItem {
                    Image(systemName: "house.fill")
                    Text("Home")
                }
            
//            Profile(student: FetchService().student!)
//                .tabItem {
//                    Image(systemName: "person.fill")
//                    Text("User")
//                }
        }
        .onAppear{
            Task{
                currentStudent = try FetchService().fetchCurrentStudent() ?? FetchService().student!
            }
        }
        .preferredColorScheme(.light)
    }
}

#Preview {
    HomeTab(showSLView: .constant(true))
}
