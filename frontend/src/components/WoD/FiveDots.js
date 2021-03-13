import React from 'react'
import CheckDot from './CheckDot'
import './FiveDots.css'

export default class FiveDots extends React.Component
{
	render()
	{
		return (
			<div className="fiveDots">
				<CheckDot className="one" checked={this.props.value > 0} onClick={() => this.props.valueChangedHandler(1)} />
				<CheckDot className="two" checked={this.props.value > 1} onClick={() => this.props.valueChangedHandler(2)} />
				<CheckDot className="three" checked={this.props.value > 2} onClick={() => this.props.valueChangedHandler(3)} />
				<CheckDot className="four" checked={this.props.value > 3} onClick={() => this.props.valueChangedHandler(4)} />
				<CheckDot className="five" checked={this.props.value > 4} onClick={() => this.props.valueChangedHandler(5)} />
			</div>
		)
	}
}
