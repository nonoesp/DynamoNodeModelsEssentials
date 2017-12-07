# DynamoNodeModelsEssentials

This project provides a Visual Studio template for advanced Dynamo package development using Node Models.

## Nodes

### Essentials

- AstReusingFunctionCall. This node displays a desirable way of using the AST where function calls are executed only once and return values are re-used. (Read node summary for detailed explanation.)
- AstNotReusingFunctionCall. This node displays a non-desirable way of using the AST where function calls are executed multiple times and return values are not re-used. (Read node summary for detailed explanation.)
- DataBridge. How the data bridge pattern works in order to pass the data connected to input ports (or the data of generated for the output ports) to the `NodeModel` instance.
- Error. Throw a custom warning when something fails.
- Events. Execute a custom 
- MultiOperation. A node that calls four different functions and returns the resulting values, i.e., a multi-return `NodeModel`.
- Multiply. A node that calls a function with two inputs and returns the resulting value.
- Timeout. Determine the maximum duration a node can run for (and time out if it surpasses it).

### Geometry

- CustomPreview. Specify how the viewport should render a custom C# class when it's returned by a Dynamo node.
- SurfaceFrom4Points.
- UVPlanesOnSurface.
- WobblySurface.

### UI

- Button.
- ButtonFunction.
- CopyableWatch.
- Slider.
- SliderBound.
- State.

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
