//
//  HomeTab.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct HomeTab: View {
    var body: some View {
        TabView {
            Home()
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
        .preferredColorScheme(.light)
    }
}

#Preview {
    HomeTab()
}
