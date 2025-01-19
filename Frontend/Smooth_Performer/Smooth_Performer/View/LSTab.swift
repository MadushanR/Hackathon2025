//
//  TabView.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct LSTab: View {
    @Binding var showSLView:Bool
    @State var login = true
    var body: some View {
        if login{
            Login(showSLView: $showSLView, changeView: $login)
        }else{
            SignUp(showSLView: $showSLView, changeView: $login)
        }
    }
}

#Preview {
    LSTab(showSLView: .constant(false))
}
