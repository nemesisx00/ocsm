import React from 'react'
import CheckDot from '../CheckDot'
import './WyrdTracker.css'

export default class WyrdTracker extends React.Component
{
	render()
	{
		return (
			<div className="wyrdTracker">
				<p className="label">Wyrd</p>
				<div className="wyrd">
					<CheckDot className="one" checked={this.props.wyrd > 0} />
					<CheckDot className="two" checked={this.props.wyrd > 1} />
					<CheckDot className="three" checked={this.props.wyrd > 2} />
					<CheckDot className="four" checked={this.props.wyrd > 3} />
					<CheckDot className="five" checked={this.props.wyrd > 4} />
					<CheckDot className="six" checked={this.props.wyrd > 5} />
					<CheckDot className="seven" checked={this.props.wyrd > 6} />
					<CheckDot className="eight" checked={this.props.wyrd > 7} />
					<CheckDot className="nine" checked={this.props.wyrd > 8} />
					<CheckDot className="ten" checked={this.props.wyrd > 9} />
				</div>
			</div>
		)
	}
}
