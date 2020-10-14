module Main

open Elmish
open Fable.Import
open Browser 

type Page =
    | Home of Home.Model
    | Other of Other.Model
    | NotFound

type Model =
    { ActivePage : Page
      CurrentRoute : Router.Route option }

type Msg =
    | HomeMsg of Home.Msg
    | OtherMsg of Other.Msg

let private setRoute (optRoute: Router.Route option) model =
    let model = { model with CurrentRoute = optRoute }
    match optRoute with
    | None ->
        { model with ActivePage = NotFound }, Cmd.none

    | Some Router.Route.Home ->
        let (homeModel, homeCmd) = Home.init ()
        { model with ActivePage = Home homeModel }, Cmd.map HomeMsg homeCmd

    | Some (Router.Route.Other id) ->
        let (otherModel, otherCmd) = Other.init id
        { model with ActivePage = Other otherModel }, Cmd.map OtherMsg otherCmd

let init (location : Router.Route option) =
    setRoute location
        { ActivePage = NotFound
          CurrentRoute = None }

let update (msg : Msg) (model : Model) =
    match model.ActivePage, msg with
    | NotFound, _ ->
        model, Cmd.none

    | Home homeModel, HomeMsg homeMsg ->
        let (homeModel, homeCmd) = Home.update homeMsg homeModel
        { model with ActivePage = Home homeModel }, Cmd.map HomeMsg homeCmd

    | Other otherModel, OtherMsg otherMsg->
        let (otherModel, otherCmd) = Other.update otherMsg otherModel
        { model with ActivePage = Other otherModel }, Cmd.map OtherMsg otherCmd

    | _, msg ->
        console.warn ("Message discarded:\n", string msg)
        model, Cmd.none

open Fable.React

let view (model : Model) (dispatch : Dispatch<Msg>) =
    match model.ActivePage with
    | NotFound ->
        str "404 Page not found"

    | Home homeModel ->
        Home.view homeModel (HomeMsg >> dispatch)

    | Other otherModel ->
        Other.view otherModel (OtherMsg >> dispatch)

open Elmish.React
open Elmish.UrlParser

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
|> Program.toNavigable (parseHash Router.routeParser) setRoute // use parsePath instead of parseHash
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
