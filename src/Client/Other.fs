module Other

open Elmish
open Fable.Remoting.Client
open Shared

type Model =
    { id : int }

type Msg =
    | Nothing

let init id : Model * Cmd<Msg> =
    let model =
        { id = id }
    model, Cmd.none

let update (msg: Msg) (model: Model): Model * Cmd<Msg> =
    match msg with
    | Nothing ->
        model, Cmd.none 

open Fable.React
open Fable.React.Props

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        span [] [ str <| string model.id ]
    ]
