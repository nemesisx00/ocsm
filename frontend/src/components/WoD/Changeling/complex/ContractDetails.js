import './ContractDetails.css'
import React from 'react'
import FiveDots from '../../FiveDots'

export default class ContractDetails extends React.Component
{
	render()
	{
		return this.generateDisplayView()
	}
	
	generateDisplayView()
	{
		let {mods, sits} = this.generateModifierLists()
		
		return (
			<div className="contractDetails">
				<div className="row">
					{this.props.contract.label}
					<FiveDots value={this.props.contract.dots} />
				</div>
				<hr />
				<div className="row">{this.props.contract.description}</div>
				<hr />
				<div className="row indent">Cost: {this.props.contract.cost}</div>
				<div className="row indent">Dice Pool: {this.props.contract.dicePool}</div>
				<div className="row indent">Action: {this.props.contract.action}</div>
				<div className="row indent">Catch: {this.props.contract.catch}</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="row indent">Dramatic Failure: {this.props.contract.results.dramatic}</div>
					<hr />
					<div className="row indent">Failure: {this.props.contract.results.failure}</div>
					<hr />
					<div className="row indent">Success: {this.props.contract.results.success}</div>
					<hr />
					<div className="row indent">Exceptional Success: {this.props.contract.results.exceptional}</div>
				</div>
				<hr />
				<div className="row">Suggested Modifiers</div>
				<hr />
				<div className="row indent">
					<div className="column">
						<div>Modifier</div>
						{mods}
					</div>
					<div className="column">
						<div>Situation</div>
						{sits}
					</div>
				</div>
			</div>
		)
	}
	
	generateModifierLists()
	{
		let out = {
			mods: [],
			sits: []
		}
		
		this.props.contract.modifiers.forEach(obj => {
			out.mods.push(
				(<div>{obj.modifier}</div>)
			)
			out.sits.push(
				(<div>{obj.situation}</div>)
			)
		})
		
		return out
	}
}
