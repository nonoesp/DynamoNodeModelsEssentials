# DynamoNodeModelsEssentials

This project provides a Visual Studio template for advanced Dynamo package development using Node Models.

## Nodes

### Essentials

- [AstReusingFunctionCall](src/Essentials/NodeModelsEssentials/EssentialsAstReusingFunctionCall.cs). This node displays a desirable way of using the AST where function calls are executed only once and return values are re-used. (Read node summary for detailed explanation.)
- [AstNotReusingFunctionCall](src/Essentials/NodeModelsEssentials/EssentialsAstNotReusingFunctionCall.cs). This node displays a non-desirable way of using the AST where function calls are executed multiple times and return values are not re-used. (Read node summary for detailed explanation.)
- [DataBridge](src/Essentials/NodeModelsEssentials/EssentialsDataBridge.cs). How the data bridge pattern works in order to pass the data connected to input ports (or the data of generated for the output ports) to the `NodeModel` instance.
- [Error](src/Essentials/NodeModelsEssentials/EssentialsError.cs). Throw a custom warning when something fails.
- [Events](src/Essentials/NodeModelsEssentials/EssentialsEvents.cs). Execute a custom 
- [MultiOperation](src/Essentials/NodeModelsEssentials/EssentialsMultiOperation.cs). A node that calls four different functions and returns the resulting values, i.e., a multi-return `NodeModel`.
- [Multiply](src/Essentials/NodeModelsEssentials/EssentialsMultiply.cs). A node that calls a function with two inputs and returns the resulting value.
- [Timeout](src/Essentials/NodeModelsEssentials/EssentialsTimeout.cs). Determine the maximum duration a node can run for (and time out if it surpasses it).

### Geometry

- [CustomPreview](src/Essentials/NodeModelsEssentials/GeometryCustomPreview.cs). Specify how the viewport should render a custom C# class when it's returned by a Dynamo node.
- [SurfaceFrom4Points](src/Essentials/NodeModelsEssentials/GeometrySurfaceFrom4Points.cs).
- [UVPlanesOnSurface](src/Essentials/NodeModelsEssentials/GeometryUVPlanesOnSurface.cs).
- [WobblySurface](src/Essentials/NodeModelsEssentials/GeometryWobblySurface.cs).

### UI

- [Button](src/Essentials/NodeModelsEssentials/UIButton.cs).
- [ButtonFunction](src/Essentials/NodeModelsEssentials/UIButtonFunction.cs).
- [CopyableWatch](src/Essentials/NodeModelsEssentials/UICopyableWatch.cs).
- [Slider](src/Essentials/NodeModelsEssentials/UISlider.cs).
- [SliderBound](src/Essentials/NodeModelsEssentials/UISliderBound.cs).
- [State](src/Essentials/NodeModelsEssentials/UIState.cs).

## Acknowledgments

These nodes document, in one way or another, my own learning on how to create nodes for Dynamo.

The initial template and samples provided in this project were inspired by [DynamoDS/DynamoSamples](https://github.com/DynamoDS/DynamoSamples) and [teocomi/HellowDynamo](https://github.com/teocomi/HelloDynamo). I highly recommend you to take a look at these for further learning.

[Jose Luis Garcia del Castillo](http://github.com/garciadelcastillo) wrote the Surface function samples.

Big thanks to the [Dynamo](http://dynamobim.org) development team.

## License

DynamoNodeModelsEssentials is licensed under the MIT license. (http://opensource.org/licenses/MIT)

## Me

I'm [Nono Martínez Alonso](http://nono.ma) ([nono.ma](http://nono.ma)), a computational designer with a penchant for design, code, and simplicity.  
I tweet at [@nonoesp](http://www.twitter.com/nonoesp), sketch at [@sketch.nono.ma](http://sketch.nono.ma), and write at [Getting Simple](http://gettingsimple.com/). If you use this, I would love to hear about it. Thanks!
