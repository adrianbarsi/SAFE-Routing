module Router

let inline (</>) a b = a + "/" + string b

type Route =
    | Home
    | Other of int

let toHash (route : Route) =
    match route with
    | Other id -> "other" </> id
    | Home -> "home"

open Elmish.UrlParser

let routeParser : Parser<Route -> Route, Route> =
    oneOf [ // Auth Routes
            map Other (s "other" </> i32)
            map Home (s "home")
            // Default Route
            map Home top ]