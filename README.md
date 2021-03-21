# Kodefabrikken

Helpers for missing features in C#.

## Kodefabrikken.Types

Types simplifying kode.

### Option
Non-mutable wrapper for optional values that avoids any Nullreference Exception.

Create with
``` C#
var option1 = new Option<int>(3);
var option2 = Option<int>.Empty; // or new Option<int>();
var option3 = new Option<SomeClass>(someClassValue);
// constructing with null, Option<> or Nullable<> throws exception

var option4 = object.ToOption();
var option5 = nullable.ToOption();
var option6 = value.ToOption();
```


Check value
``` C#
if(option.HasValue) ...
option.IfValue(p => Console.WriteLine(p));
option.IfValue(p => ...).Else(() => Console.WriteLine("empty"));
```

Coalesce to value
``` C#
var x = option.Coalesce(3);
var x = option.Coalesce(() => 7);
```

Compare
``` C#
// some special cases

var option1 = Option<someType>.Empty;
option1.Equals(null);

var option2 = Option<int>(3);
option2.Equals(3);
```

Convert to 'native' types
``` C#
option1.ToObject(); // for reference type
option2.ToNullable(); // for value type
```

 