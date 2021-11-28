import React from 'react'
import Checker from '../core/Checker'
import './FiveDots.css'

export default class FiveDots extends React.Component
{
	render()
	{
		return (
			<div className="fiveDots">
				<Checker type={Checker.Types.Circle} checked={this.props.value > 0} clickHandler={() => this.props.valueChangedHandler(1)} />
				<Checker type={Checker.Types.Circle} checked={this.props.value > 1} clickHandler={() => this.props.valueChangedHandler(2)} />
				<Checker type={Checker.Types.Circle} checked={this.props.value > 2} clickHandler={() => this.props.valueChangedHandler(3)} />
				<Checker type={Checker.Types.Circle} checked={this.props.value > 3} clickHandler={() => this.props.valueChangedHandler(4)} />
				<Checker type={Checker.Types.Circle} checked={this.props.value > 4} clickHandler={() => this.props.valueChangedHandler(5)} />
			</div>
		)
	}
}
