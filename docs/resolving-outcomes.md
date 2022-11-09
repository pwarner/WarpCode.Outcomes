# Resolving Outcomes

When you've stuffed a value or a problem into an` Outcome<T>`, you might be surprised to find you can't directly them because the `Outcome<T>` type does not expose either a Value or Problem directly. 

Nor does it expose any boolean indicators such as `IsSuccess` or `HasProblem`.

This is a deliberate design decision to adhere to a `Monad` design pattern from the functional programming word. A Monad usually appears in the form of a kind of wrapper that augments an inner value with behaviour, but it's also a type of contract