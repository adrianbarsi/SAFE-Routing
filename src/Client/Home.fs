module Home

open Elmish
open Fable.Remoting.Client
open Shared
open Elmish.Navigation

type Model =
    { count : int }

type Msg =
    | Increment
    | Decrement
    | InitializeCount of int
    | Link of Router.Route

let api =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<Api>

let init(): Model * Cmd<Msg> =
    let model =
        { count = 0}
    model, Cmd.OfAsync.perform api.getNumber () InitializeCount

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | Increment ->
        { model with count = model.count + 1}, Cmd.none
    
    | Decrement ->
        { model with count = model.count - 1}, Cmd.none

    | InitializeCount initialCount ->
        { model with count = initialCount}, Cmd.none
    
    | Link route ->
        model, Navigation.newUrl (Router.toHash route)

open Fable.React
open Fable.React.Props

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        button [ OnClick (fun _ -> dispatch Increment) ] [ str "Increment" ]
        span [] [ model.count |> string |> str ]
        button [ OnClick (fun _ -> dispatch Decrement) ] [ str "Decrement" ]
        br []
        a [ OnClick (fun _ -> Link (Router.Other 5) |> dispatch) ] [ str "Other" ]
        br []
        a [ OnClick (fun _ -> Link Router.Home |> dispatch) ] [ str "Home" ]
    ]
