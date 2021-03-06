//
//  CommandServiceTests.Raw.Conditions.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Remora.Commands.Extensions;
using Remora.Commands.Services;
using Remora.Commands.Tests.Data.Conditions;
using Remora.Commands.Tests.Data.Modules;
using Xunit;

namespace Remora.Commands.Tests.Services
{
    public partial class CommandServiceTests
    {
        public partial class Raw
        {
            /// <summary>
            /// Tests conditional commands.
            /// </summary>
            public class Conditions
            {
                /// <summary>
                /// Tests whether the command service can execute a command with a group condition.
                /// </summary>
                /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
                [Fact]
                public async Task CanExecuteCommandWithGroupCondition()
                {
                    var services = new ServiceCollection()
                        .AddCommands()
                        .AddCommandGroup<CommandGroupWithGroupCondition>()
                        .AddCondition<GroupCondition>()
                        .BuildServiceProvider();

                    var commandService = services.GetRequiredService<CommandService>();
                    var executionResult = await commandService.TryExecuteAsync
                    (
                        "test method",
                        services
                    );

                    Assert.True(executionResult.IsSuccess);
                }

                /// <summary>
                /// Tests whether the command service can execute a command with a group condition.
                /// </summary>
                /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
                [Fact]
                public async Task CanExecuteCommandWithOuterGroupCondition()
                {
                    var services = new ServiceCollection()
                        .AddCommands()
                        .AddCommandGroup<NestedCommandGroupWithOuterGroupCondition>()
                        .AddCondition<GroupCondition>()
                        .BuildServiceProvider();

                    var commandService = services.GetRequiredService<CommandService>();
                    var executionResult = await commandService.TryExecuteAsync
                    (
                        "test method",
                        services
                    );

                    Assert.True(executionResult.IsSuccess);
                }

                /// <summary>
                /// Tests whether the command service can execute a command with a method condition.
                /// </summary>
                /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
                [Fact]
                public async Task CanExecuteCommandWithMethodCondition()
                {
                    var services = new ServiceCollection()
                        .AddCommands()
                        .AddCommandGroup<ConditionalCommandGroup>()
                        .AddCondition<MethodCondition>()
                        .BuildServiceProvider();

                    var commandService = services.GetRequiredService<CommandService>();
                    var executionResult = await commandService.TryExecuteAsync
                    (
                        "test method-condition",
                        services
                    );

                    Assert.True(executionResult.IsSuccess);
                }

                /// <summary>
                /// Tests whether the command service can execute a command with a parameter condition.
                /// </summary>
                /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
                [Fact]
                public async Task CanExecuteCommandWithParameterCondition()
                {
                    var services = new ServiceCollection()
                        .AddCommands()
                        .AddCommandGroup<ConditionalCommandGroup>()
                        .AddCondition<ParameterCondition>()
                        .BuildServiceProvider();

                    var commandService = services.GetRequiredService<CommandService>();
                    var executionResult = await commandService.TryExecuteAsync
                    (
                        "test parameter-condition booga",
                        services
                    );

                    Assert.True(executionResult.IsSuccess);

                    executionResult = await commandService.TryExecuteAsync
                    (
                        "test parameter-condition wooga",
                        services
                    );

                    Assert.False(executionResult.IsSuccess);
                }
            }
        }
    }
}
