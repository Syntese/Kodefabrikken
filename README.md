# Kodefabrikken

Helpers for missing features in C#.

## Kodefabrikken.Types

Types simplifying kode.

### Optional
Non-mutable wrapper for optional values that avoids any Nullreference Exception.

Create with
``` C#
var optional1 = new Optional<int>(3);
var optional2 = Optional<int>.Empty; // or new Option<int>();
var optional3 = new Optional<SomeClass>(someClassValue);
// constructing with null, Optional<> or Nullable<> throws exception

var optional4 = object.ToOptional();
var optional5 = nullable.ToOptional();
var optional6 = value.ToOptional();
```


Check value
``` C#
if(optional.HasValue) ...
optional.IfValue(p => Console.WriteLine(p));
optional.IfValue(p => ...).Else(() => Console.WriteLine("empty"));
```

Coalesce to value
``` C#
var x = optional.Coalesce(3);
var x = optional.Coalesce(() => 7);
```

Compare
``` C#
// some special cases

var optional1 = Optional<someType>.Empty;
optional1.Equals(null);

var optional2 = Optional<int>(3);
optional2.Equals(3);
```

Convert to 'native' types
``` C#
optional1.ToObject(); // for reference type
optional2.ToNullable(); // for value type
```

 