import React from 'react'
import CheckDot from './CheckDot'
import './FiveDots.css'

export default class FiveDots extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			value: props.value
		}
	}
	
	componentDidUpdate()
	{
		if(this.props.value !== this.state.value)
			this.props.valueChangedHandler(this.state.value)
	}
	
	clickHandler(value)
	{
		this.setState(() => {
			return {
				//Handle clicking a "checked" dot to "uncheck" it
				value: this.state.value === value
						? value - 1
						: value
			}
		})
	}
	
	render()
	{
		return (
			<div className="fiveDots">
				<CheckDot className="one" checked={this.state.value > 0} onClick={() => this.clickHandler(1)} />
				<CheckDot className="two" checked={this.state.value > 1} onClick={() => this.clickHandler(2)} />
				<CheckDot className="three" checked={this.state.value > 2} onClick={() => this.clickHandler(3)} />
				<CheckDot className="four" checked={this.state.value > 3} onClick={() => this.clickHandler(4)} />
				<CheckDot className="five" checked={this.state.value > 4} onClick={() => this.clickHandler(5)} />
			</div>
		)
	}
}
