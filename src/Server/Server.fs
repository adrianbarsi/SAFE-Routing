module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared

let getNumber () : Async<int> =
    async {
        return 10
    }

let api : Api = { getNumber = getNumber }

let remoteApi =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue api
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router remoteApi
        memory_cache
        use_static "public"
        use_gzip
    }

run app

//Using path based routing instead of hash tag

(*
I'm not quite sure how to get my routing working correctly. I'm using Program.toNavigable with parsePath and the Navigation module. I can create a link and use it like this: a [ OnClick (fun _ -> Link Router.Home |> dispatch) ] [ str "Home" ] with the part in update here: Link route -> model, Navigation.newUrl (Router.toRoute route). That all works correctly. The problem is when I try to manually enter the route in my browser I get: "Cannot GET /home" as an error message. I think it's because It's trying to go to the server and the server doesn't have an endpoint for /home. This error also happens if I try to refresh the page. Basically the only way it actually changes routes is if I click a link that I made specifically for that purpose.
If I use parseHash instead of parsePath everything works correctly but I would rather use paths instead of hashes
petsquiToday at 2:12 AM
@AmusingGnome whatever serves your app needs to be conifgured properly for spa and client side routing
AmusingGnomeToday at 2:14 AM
Ok so Saturn needs to be set up somehow
AdenyToday at 10:32 AM
@AmusingGnome how about /#/Home?
ZerotToday at 10:33 AM
@AmusingGnome yes. When using html5 routing, then your webserver needs to return the index page for basically all routes
AmusingGnomeToday at 5:03 PM
@Adeny Yeah that works
@Zerot Doesn't that kind of go against what an spa is though? How would I connect that with elmish? Ideally it would just send an empty html and run the init from there.
ZerotToday at 5:06 PM
no? With html5 routing the location state gets updated when clicking a link. But directly navigating to a location means that the browser will send of a request to get the page
so it will not request the page when the user is using the spa, but if the user hits refresh or directly enters an address then the browser will do a request
you can intercept it and return a cached version using service workers, but the server will always need to be set up to return the same page for all those routes
AmusingGnomeToday at 5:11 PM
Ok thanks for the info
ZerotToday at 5:16 PM
so basically what you can do is change the 404 handler in your webserver to always return your index page. That way your SPA will load regardless of how the user got there and the SPA can then read the location and show the correct view
the reason why it works for hash router is that the hash does not get send to the server. So your base route will always be the same
AmusingGnomeToday at 5:17 PM
Oh that makes sense. I should be able to do that thanks.
*)