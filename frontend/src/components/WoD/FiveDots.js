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
		
		this.mainDiv = React.createRef()
	}
	
	clickHandler()
	{
		this.setState(() => {
			return { value: this.mainDiv.current.querySelectorAll('.checkDot.checked').length }
		})
	}
	
	render()
	{
		return (
			<div className="fiveDots" ref={this.mainDiv} onClick={() => this.clickHandler()}>
				<CheckDot className="one" checked={this.state.value > 0} />
				<CheckDot className="two" checked={this.state.value > 1} />
				<CheckDot className="three" checked={this.state.value > 2} />
				<CheckDot className="four" checked={this.state.value > 3} />
				<CheckDot className="five" checked={this.state.value > 4} />
			</div>
		)
	}
	
	get value()
	{
		//Make sure we're getting the most up-to-date value
		this.setState(() => {
			return { value: this.mainDiv.current.querySelectorAll('.checkDot.checked').length }
		})
		
		return this.state.value
	}
	
	set value(val)
	{
		this.setState(() => {
			return { value: val }
		})
	}
}
