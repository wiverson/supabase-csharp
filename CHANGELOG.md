﻿# Changelog

## 0.12.0 - 2023-06-25

- Update dependency: `gotrue-csharp@4.1.0`
    - **Minor** [#66](https://github.com/supabase-community/gotrue-csharp/pull/66) - Separates out Admin JWT
      functionality into a separate `AdminClient`
    - [#67](https://github.com/supabase-community/gotrue-csharp/pull/67) - Adds shutdown method which terminates the
      background refresh threads.
    - Movement of much of the documentation for methods out of their classes and into their interfaces.
    - Language features locked to C#9
- Update dependency:`postgrest-csharp@3.2.3`
    - [#69](https://github.com/supabase-community/postgrest-csharp/pull/69) Locks language version to C#9
    - [#68](https://github.com/supabase-community/postgrest-csharp/pull/68) Makes RPC parameters optional

Thanks [@wiverson](https://github.com/wiverson) for the work in this release!

## 0.11.1 - 2023-06-10

- Update dependencies: `functions-csharp@1.3.1`, `gotrue-csharp@4.0.4`, `postgrest-csharp@3.2.2`,
  `realtime-csharp@6.0.3`, `supabase-storage-csharp@1.3.2`, `supabase-core@0.0.3`
    - Namespaces assembly names to make them unique among other dependencies, i.e: `Core.dll`
      becomes `Supabase.Core.dll` which will hopefully prevent future collisions.

## 0.11.0 - 2023-05-24

- Update dependency: postgrest-csharp@3.2.0
    - General codebase and QOL improvements. Exceptions are generally thrown through `PostgrestException` now instead
      of `Exception`. A `FailureHint.Reason` is provided with failures if possible to parse.
    - `AddDebugListener` is now available on the client to help with debugging
    - Merges [#65](https://github.com/supabase-community/postgrest-csharp/pull/65) Cleanup + Add better exception
      handling
    - Merges [#66](https://github.com/supabase-community/postgrest-csharp/pull/66) Local test Fixes
    - Fixes [#67](https://github.com/supabase-community/postgrest-csharp/issues/67) Postgrest Reference attribute is
      producing StackOverflow for circular references
- Update dependency: gotrue-csharp@4.0.2
    - [#58](https://github.com/supabase-community/gotrue-csharp/issues/58) - Add support for the `reauthentication`
      endpoint which allows for secure password changes.
- Update dependency: realtime-csharp@6.0.1
    - Updates publishing action for future packages, includes README and icon.
    - Merges [#28](https://github.com/supabase-community/realtime-csharp/pull/28)
      and [#30](https://github.com/supabase-community/realtime-csharp/pull/30)
    - The realtime client now takes a "fail-fast" approach. On establishing an initial connection, client will throw
      a `RealtimeException` in `ConnectAsync()` if the socket server is unreachable. After an initial connection has
      been
      established, the **client will continue attempting reconnections indefinitely until disconnected.**
    - [Major, New] C# `EventHandlers` have been changed to `delegates`. This should allow for cleaner event data access
      over
      the previous subclassed `EventArgs` setup. Events are scoped accordingly. For example, the `RealtimeSocket` error
      handlers will receive events regarding socket connectivity; whereas the `RealtimeChannel` error handlers will
      receive
      events according to `Channel` joining/leaving/etc. This is implemented with the following methods prefixed by (
      Add/Remove/Clear):
        - `RealtimeBroadcast.AddBroadcastEventHandler`
        - `RealtimePresence.AddPresenceEventHandler`
        - `RealtimeSocket.AddStateChangedHandler`
        - `RealtimeSocket.AddMessageReceivedHandler`
        - `RealtimeSocket.AddHeartbeatHandler`
        - `RealtimeSocket.AddErrorHandler`
        - `RealtimeClient.AddDebugHandler`
        - `RealtimeClient.AddStateChangedHandler`
        - `RealtimeChannel.AddPostgresChangeHandler`
        - `RealtimeChannel.AddMessageReceivedHandler`
        - `RealtimeChannel.AddErrorHandler`
        - `Push.AddMessageReceivedHandler`
    - [Major, new] `ClientOptions.Logger` has been removed in favor of `Client.AddDebugHandler()` which allows for
      implementing custom logging solutions if desired.
        - A simple logger can be set up with the following:
      ```c#
      client.AddDebugHandler((sender, message, exception) => Debug.WriteLine(message));
      ```
    - [Major] `Connect()` has been marked `Obsolete` in favor of `ConnectAsync()`
    - Custom reconnection logic has been removed in favor of using the built-in logic from `Websocket.Client@4.6.1`.
    - Exceptions that are handled within this library have been marked as `RealtimeException`s.
    - The local, docker-composed test suite has been brought back (as opposed to remotely testing on live supabase
      servers)
      to test against.
    - Comments have been added throughout the entire codebase and an `XML` file is now generated on build.

## 0.10.0 - 2023-05-14

- Changes options to require `Supabase.SupabaseOptions.SessionPersistor` from using `ISupabaseSessionHandler`
  to `IGotrueSessionPersistance<Session>` (these are now synchronous operations).
- Update dependency: gotrue-csharp@4.0.1
    - [#60](https://github.com/supabase-community/gotrue-csharp/pull/60) - Add interfaces, bug fixes, additional error
      reason detection. Thanks [@wiverson](https://github.com/wiverson)!
    - [#57](https://github.com/supabase-community/gotrue-csharp/pull/57) Refactor exceptions, code cleanup, and move to
      delegate auth state changes
        - Huge thank you to [@wiverson](https://github.com/wiverson) for his help on this refactor and release!
        - Changes
            - Exceptions have been simplified to a single `GotrueException`. A `Reason` field has been added
              to `GotrueException` to clarify what happened. This should also be easier to manage as the Gotrue
              server API & messages evolve.
            - The session delegates for `Save`/`Load`/`Destroy` have been simplified to no longer require `async`.
            - Console logging in a few places (most notable the background refresh thread) has been removed
              in favor of a notification method. See `Client.AddDebugListener()` and the test cases for examples.
              This will allow you to implement your own logging strategy (write to temp file, console, user visible
              err console, etc).
            - The client now more reliably emits AuthState changes.
            - There is now a single source of truth for headers in the stateful Client - the `Options` headers.
        - New feature:
            - Added a `Settings` request to the stateless API only - you can now query the server instance to
              determine if it's got the settings you need. This might allow for things like a visual
              component in a tool to verify the GoTrue settings are working correctly, or tests that run differently
              depending on the server configuration.
        - Implementation notes:
            - Test cases have been added to help ensure reliability of auth state change notifications
              and persistence.
            - Persistence is now managed via the same notifications as auth state change

## 0.9.1 - 2023-04-28

- Update dependency: gotrue-csharp@3.1.1
    - Implements `SignInWithIdToken` for Apple/Google signing from LW7. A HUGE thank you
      to [@wiverson](https://github.com/wiverson)!
- Update dependency: realtime-csharp@5.0.5
    - Re: [#27](https://github.com/supabase-community/realtime-csharp/issues/27) `PostgresChangesOptions` was not
      setting `listenType` in constructor. Thanks [@Kuffs2205](https://github.com/Kuffs2205)
- Update dependency: supabase-storage-csharp@1.2.10
    - Re: [#7](https://github.com/supabase-community/storage-csharp/issues/7) Implements a `DownloadPublicFile` method.

## 0.9.0 - 2023-04-12

- Update dependency: gotrue-csharp@3.1.0
    - [Minor] Implements PKCE auth flow. SignIn using a provider now returns an instance of `ProviderAuthState` rather
      than a `string`.

- Update dependency: supabase-storage-csharp@1.2.9
    - Implements storage features from LW7:
        - feat: custom file size limit and mime types at bucket
          level [supabase/storage-js#151](https://github.com/supabase/storage-js/pull/151) file size and mime type
          limits per bucket
        - feat: quality option, image
          transformation [supabase/storage-js#145](https://github.com/supabase/storage-js/pull/152) quality option for
          image transformations
        - feat: format option for webp
          support [supabase/storage-js#142](https://github.com/supabase/storage-js/pull/142) format option for image
          transformation

## 0.8.8 - 2023-03-29

- Update dependency: gotrue-csharp@3.0.6
    - Supports adding `SignInOptions` (i.e. `RedirectTo`) on `OAuth Provider` SignIn requests.

## 0.8.7 - 2023-03-23

- Update dependency: realtime-csharp@5.0.4
    - Re: [#26](https://github.com/supabase-community/realtime-csharp/pull/26) - Fixes Connect() not returning callback
      result when the socket isn't null. Thanks [@BlueWaterCrystal](https://github.com/BlueWaterCrystal)!

## 0.8.6 - 2023-03-23

- Update dependency: supabase-storage-csharp@1.2.8
    - [Merge #5](https://github.com/supabase-community/storage-csharp/pull/5) Added search string as an optional search
      parameter. Thanks [@ElectroKnight22](https://github.com/ElectroKnight22)!

## 0.8.5 - 2023-03-10

- Update dependency: realtime-csharp@5.0.3
    - Re: [#25](https://github.com/supabase-community/realtime-csharp/issues/25) - Support Channel being resubscribed
      after having been unsubscribed, fixes rejoin timer being erroneously called on channel `Unsubscribe`.
      Thanks [@Kuffs2205](https://github.com/Kuffs2205)!

## 0.8.4 - 2023-03-03

- Update dependency: supabase-storage-csharp@1.2.7
    - Re: [#4](https://github.com/supabase-community/storage-csharp/issues/4) Implementation for `ClientOptions` which
      supports specifying Upload, Download, and Request timeouts.
- Update dependency: realtime-csharp@5.0.2
    - Re: [#24](https://github.com/supabase-community/realtime-csharp/issues/24) - Fixes join failing until reconnect
      happened + adds access token push on channel join. Big thank you to [@Honeyhead](https://github.com/honeyhead) for
      the help debugging and identifying!

## 0.8.3 - 2023-02-26

- Update dependency: supabase-storage-csharp@1.2.5
    - Provides fix
      for [supabase-community/supabase-csharp#54](https://github.com/supabase-community/supabase-csharp/issues/54) -
      Dynamic headers were always being overwritten by initialized token headers, so the storage client would not
      receive user's access token as expected.
    - Provides fix for upload progress not reporting
      in [supabase-community/storage-csharp#3](https://github.com/supabase-community/storage-csharp/issues/3)
- Update dependency: gotrue-csharp@3.0.5
    - Fixes [#44](https://github.com/supabase-community/gotrue-csharp/issues/44) - refresh timer should automatically
      reattempt (interval of 5s) for HTTP exceptions - gracefully exits on invalid refresh and triggers
      an `AuthState.Changed` event

## 0.8.2 - 2023-02-26

- Update dependency: supabase-storage-csharp@1.2.4
    - `UploadOrUpdate` now appropriately throws request exceptions

## 0.8.1 - 2023-02-06

- Update dependency: realtime-csharp@5.0.1
    - Re: [#22](https://github.com/supabase-community/realtime-csharp/issues/22) - `SerializerSettings` were not being
      passed to `PostgresChangesResponse` - Thanks [@Shenrak](https://github.com/Shenrak) for the help debugging!

## 0.8.0 - 2023-01-31

- Update dependency: realtime-csharp@5.0.0
    - Re: [#21](https://github.com/supabase-community/realtime-csharp/pull/21) Provide API for `presence`, `broadcast`
      and `postgres_changes`
        - [Major, New] `Channel.PostgresChanges` event will receive the wildcard `*` changes event,
          not `Channel.OnMessage`.
        - [Major] `Channel.OnInsert`, `Channel.OnUpdate`, and `Channel.OnDelete` now conform to the server's payload
          of `Response.Payload.**Data**`
        - [Major] `Channel.OnInsert`, `Channel.OnUpdate`, and `Channel.OnDelete` now return `PostgresChangesEventArgs`
        - [Minor] Rename `Channel` to `RealtimeChannel`
        - Supports better handling of disconnects in `RealtimeSocket` and adds a `Client.OnReconnect` event.
        - [Minor] Moves `ChannelOptions` to `Channel.ChannelOptions`
        - [Minor] Moves `ChannelStateChangedEventArgs` to `Channel.ChannelStateChangedEventArgs`
        - [Minor] Moves `Push` to `Channel.Push`
        - [Minor] Moves `Channel.ChannelState` to `Constants.ChannelState`
        - [Minor] Moves `SocketResponse`, `SocketRequest`, `SocketResponsePayload`, `SocketResponseEventArgs`,
          and `SocketStateChangedEventArgs` to `Socket` namespace.
        - [New] Adds `RealtimeBroadcast`
        - [New] Adds `RealtimePresence`
        - [Improvement] Better handling of disconnection/reconnection
- Update dependency: postgrest-csharp@3.1.3
    - Another fix for [#61](https://github.com/supabase-community/postgrest-csharp/issues/61) which futher typechecks
      nullable values.

## 0.7.2 - 2023-01-27

- Update dependency: gotrue-csharp@3.0.4
    - Makes `Session.CreatedAt` a publicly settable property, which should fix incorrect dates on retrieved `Session`s.
- Update dependency: postgrest-csharp@3.1.2
    - Fix [#61](https://github.com/supabase-community/postgrest-csharp/issues/61) which did not correctly parse
      Linq `Where` when encountering a nullable type.
    - Add missing support for transforming for `== null` and `!= null`

## 0.7.1 - 2023-01-17

- Update dependency: postgrest-csharp@3.1.1
    - Fix issue from supabase-community/supabase-csharp#48 where boolean model properties would not be evaluated in
      predicate expressions

## 0.7.0 - 2023-01-16

- Update dependency: postgrest-csharp@3.1.0
    - [Minor] Breaking API Change: `PrimaryKey` attribute defaults to `shouldInsert: false` as most uses will have the
      Database generate the primary key.
    - Merged [#60](https://github.com/supabase-community/postgrest-csharp/pull/60) which Added linq support
      for `Select`, `Where`, `OnConflict`, `Columns`, `Order`, `Update`, `Set`, and `Delete`

## 0.6.2 - 2022-11-22

- Update dependency: postgrest-csharp@3.0.4
    - `GetHeaders` is now passed to `ModeledResponse` and `BaseModel` so that the default `Update` and `Delete` methods
      use the latest credentials
    - `GetHeaders` is used in `Rpc` calls (re: [#39](https://github.com/supabase-community/supabase-csharp/issues/39))

## 0.6.1 - 2022-11-12

- [Hotfix] `GetHeaders` was not passing properly to `SupabaseTable` and `Gotrue.Api`

## 0.6.0 - 2022-11-12

[BREAKING CHANGES]

- `Client` is no longer a singleton, singleton interactions (if desired) are left to the developer to implement.
- `Client` supports injection of dependent clients after initialization via property:
    - `Auth`
    - `Functions`
    - `Realtime`
    - `Postgrest`
    - `Storage`
- `SupabaseModel` contains no logic but remains for backwards compatibility. (Marked `Obsolete`)
- `ClientOptions.ShouldInitializeRealtime` was removed (no longer auto initialized)
- `ClientOptions` now references an `ISupabaseSessionHandler` which specifies expected functionality for session
  persistence on Gotrue (replaces `ClientOptions.SessionPersistor`, `ClientOptions.SessionRetriever`,
  and `ClientOptions.SessionDestroyer`).
- `supabase-csharp` and all child libraries now have support `nullity`

Other Changes:

- Update dependency: functions-csharp@1.2.1
- Update dependency: gotrue-csharp@3.0.2
- Update dependency: postgrest-csharp@3.0.2
- Update dependency: realtime-csharp@4.0.1
- Update dependency: supabase-storage-csharp@1.2.3
- Update dependency: supabase-core@0.0.2

Big thank you to [@veleek](https://github.com/veleek) for his insight into these changes.

Re: [#35](https://github.com/supabase-community/supabase-csharp/issues/35), [#34](https://github.com/supabase-community/supabase-csharp/issues/34), [#23](https://github.com/supabase-community/supabase-csharp/issues/23), [#36](https://github.com/supabase-community/supabase-csharp/pull/36)

## 0.5.3 - 2022-10-11

- Update dependency: postgrest-csharp@2.1.0
    - [Minor] Breaking API change: Remove `BaseModel.PrimaryKeyValue` and `BaseModel.PrimaryKeyColumn` in favor of
      a `PrimaryKey` dictionary with support for composite keys.
    - Re: [#48](https://github.com/supabase-community/postgrest-csharp/issues/48) - Add support for derived models
      on `ReferenceAttribute`
    - Re: [#49](https://github.com/supabase-community/postgrest-csharp/issues/49) - Added `Match(T model)`

## 0.5.2 - 2022-9-13

- Update dependency: postgrest-csharp@2.0.12
    - Merged [#47](https://github.com/supabase-community/postgrest-csharp/pull/49) which added cancellation token
      support to `Table<T>` methods. Thanks [@devpikachu](https://github.com/devpikachu)!

## 0.5.1 - 2022-8-1

- Update dependency: postgrest-csharp@2.0.11
- Update dependency: supabase-storage-csharp@1.1.1

## 0.5.0 - 2022-7-17

- Update dependency: postgrest-csharp@2.0.9
- Update dependency: realtime-csharp@3.0.1
- Update dependency: supabase-storage-csharp@1.1.0
    - API Change [Breaking/Minor] Library no longer uses `WebClient` and instead leverages `HttpClient`. Progress events
      on `Upload` and `Download` are now handled with `EventHandler<float>` instead of `WebClient` EventHandlers.

## 0.4.4 - 2022-5-24

- Update dependency: gotrue-csharp@2.4.5
- Update dependency: postgrest-csharp@2.0.8

## 0.4.3 - 2022-5-13

- Update dependency: gotrue-csharp@2.4.4

## 0.4.2 - 2022-4-30

- Update dependency: gotrue-csharp@2.4.3

## 0.4.1 - 2022-4-23

- Update dependency: gotrue-csharp@2.4.2

## 0.4.0 - 2022-4-12

- Add support for functions-csharp@1.0.1, giving access to invoking Supabase's edge functions.
- Update dependency: gotrue-csharp@2.4.1

## 0.3.5 - 2022-4-11

- Update dependency: postgres-csharp@2.0.7

## 0.3.4 - 2022-03-28

- Update dependency: gotrue-csharp@2.4.0

## 0.3.3 - 2022-02-27

- Update dependency: gotrue-csharp@2.3.6
- Update dependency: supabase-storage-csharp@1.0.2

## 0.3.2 - 2022-02-18

- Update dependency: realtime-csharp@3.0.0
    - Exchange existing websocket client: [WebSocketSharp](https://github.com/sta/websocket-sharp)
      for [Marfusios/websocket-client](https://github.com/Marfusios/websocket-client) which adds support for Blazor WASM
      apps.
      Ref: [#14](https://github.com/supabase-community/realtime-csharp/pull/14)

## 0.3.1 - 2022-01-20

- Update dependency: gotrue-csharp@2.3.5
    - [#23](https://github.com/supabase-community/gotrue-csharp/pull/23) Added `redirect_url` option for MagicLink sign
      in (Thanks [@MisterJimson](https://github.com/MisterJimson))
    - [#21](https://github.com/supabase-community/gotrue-csharp/pull/21) Added SignOut method to Stateless Client (
      Thanks [@fplaras](https://github.com/fplaras))

## 0.3.0 - 2021-12-30

- Update dependency: postgrest-csharp@2.0.6
    - Add support for `NullValueHandling` to be specified on a `Column` Attribute and for it to be honored on Inserts
      and Updates. Defaults to: `NullValueHandling.Include`.
        - Implements [#38](https://github.com/supabase-community/postgrest-csharp/issues/38)
- Update dependency: realtime-csharp@2.0.8
    - Implement Upstream Realtime RLS Error Broadcast Handler
        - Implements [#12](https://github.com/supabase-community/realtime-csharp/issues/12)
    - `SocketResponse` now exposes a method: `OldModel`, that hydrates the `OldRecord` property into a model.

## 0.2.12 - 2021-12-29

- Update dependency: gotrue-csharp@2.3.3
    - `SignUp` will return a `Session` with a *populated `User` object* on an unconfirmed signup.
        - Fixes [#19](https://github.com/supabase-community/gotrue-csharp/issues/19)
        - Developers who were using a `null` check on `Session.User` will need to adjust accordingly.
- Update dependency: postgrest-csharp@2.0.5
    - Fix for [#37](https://github.com/supabase-community/postgrest-csharp/issues/37) - Return Type `minimal` would fail
      to resolve because of incorrect `Accept` headers. Added header and test to verify for future.
    - Fix for [#36](https://github.com/supabase-community/postgrest-csharp/issues/36) - Inserting/Upserting bulk records
      would fail while doing an unnecessary generic coercion.

## 0.2.11 - 2021-12-24

- Update dependency: gotrue-csharp@2.3.2 (changes CreateUser parameters to conform to `AdminUserAttributes`)
    - See [#15](https://github.com/supabase-community/supabase-csharp/issues/15)
    - See [#16](https://github.com/supabase-community/supabase-csharp/issues/16)
- Update dependency: realtime-csharp@2.0.7
    - See [#13](https://github.com/supabase-community/supabase-csharp/issues/13)

## 0.2.10 - 2021-12-23

- Update dependency: gotrue-csharp@2.3.0 (adds metadata support for user signup,
  see [#14](https://github.com/supabase/community/issues/14))

## 0.2.9 - 2021-12-9

- Separate Storage client from Supabase repo and into `storage-csharp`, `supabase-csharp` now references new repo.

## 0.2.8 - 2021-12-4

- Update gotrue-csharp to 2.2.4
    - Adds support for `ListUsers` (paginate, sort, filter), `GetUserById`, `CreateUser`, and `UpdateById`

## 0.2.7 - 2021-12-2

- Update gotrue-csharp to 2.2.3
    - Adds support for sending password resets to users.

## 0.2.6 - 2021-11-29

- Support for [#12](https://github.com/supabase-community/supabase-csharp/issues/12)
- Update realtime-csharp to 2.0.6
- Update gotrue-csharp to 2.2.2
- Add `StatelessClient` re:[#7](https://github.com/supabase-community/supabase-csharp/issues/7)
