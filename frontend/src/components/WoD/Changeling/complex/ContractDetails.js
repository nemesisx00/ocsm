import './ContractDetails.css'
import React from 'react'
import FiveDots from '../../FiveDots'

export default class ContractDetails extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			editMode: false
		}
	}
	
	render()
	{
		if(this.state.editMode)
			return this.generateEditView()
		else
			return this.generateDisplayView()
	}
	
	generateDisplayView()
	{
		let {mods, sits} = this.generateModifierLists()
		
		return (
			<div className={`contractDetails ${this.props.className}`}>
				<div className="row">
					{this.props.contract.label}
					<FiveDots value={this.props.contract.dots} valueChangedHandler={() => {}} />
					<button onClick={() => this.toggleEditMode()}>Edit</button>
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
					<div className="row indent">Dramatic Failure: {this.props.contract.resultDramatic}</div>
					<hr />
					<div className="row indent">Failure: {this.props.contract.resultFailure}</div>
					<hr />
					<div className="row indent">Success: {this.props.contract.resultSuccess}</div>
					<hr />
					<div className="row indent">Exceptional Success: {this.props.contract.resultExceptional}</div>
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
	
	generateEditView()
	{
		let mods = this.generateModifierLists()
		
		return (
			<div className={`contractDetails ${this.props.className}`}>
				<div className="row">
					<input type="text" value={this.props.contract.label} onChange={(e) => this.props.changeHandler(this.props.contract, 'label', e.target.value)} />
					<FiveDots value={this.props.contract.dots} valueChangedHandler={(arg) => this.props.changeHandler(this.props.contract, 'dots', arg)} />
					<button onClick={() => this.toggleEditMode()}>Done</button>
				</div>
				<hr />
				<div className="row">
					<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'description', e.target.value)} value={this.props.contract.description} />
				</div>
				<hr />
				<div className="row indent spacer">
					Cost:
					<input type="text" value={this.props.contract.cost} onChange={(e) => this.props.changeHandler(this.props.contract, 'cost', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Dice Pool:
					<input type="text" value={this.props.contract.dicePool} onChange={(e) => this.props.changeHandler(this.props.contract, 'dicePool', e.target.value)} />
				</div>
				<div className="row indent spacer">
					Action:
					<input type="text" value={this.props.contract.action} onChange={(e) => this.props.changeHandler(this.props.contract, 'action', e.target.value)} />
				</div>
				<div className="column indent">
					Catch:
					<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'catch', e.target.value)} value={this.props.contract.catch} />
				</div>
				<hr />
				<div className="column">
					<div className="row heading">Roll Results</div>
					<div className="column indent">
						Dramatic Failure:
						<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'resultDramatic', e.target.value)}  value={this.props.contract.resultDramatic} />
					</div>
					<hr />
					<div className="column indent">
						Failure:
						<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'resultFailure', e.target.value)} value={this.props.contract.resultFailure} />
					</div>
					<hr />
					<div className="column indent">
						Success:
						<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'resultSuccess', e.target.value)} value={this.props.contract.resultSuccess} />
					</div>
					<hr />
					<div className="column indent">
						Exceptional Success:
						<textarea onChange={(e) => this.props.changeHandler(this.props.contract, 'resultExceptional', e.target.value)} value={this.props.contract.resultExceptional} />
					</div>
				</div>
				<hr />
				<div className="row">Suggested Modifiers</div>
				<hr />
				<div className="column indent modifiers">
					<div className="row label">
						<div className="small">Modifier</div>
						<div>Situation</div>
					</div>
					{mods}
				</div>
			</div>
		)
	}
	
	generateModifierLists()
	{
		let out = null
		let mode = this.state.editMode
		
		if(mode)
		{
			out = []
			
			this.props.contract.modifiers.forEach((obj, i) => {
				out.push(
					(<div className="row" key={`modifierRow-${i}`}>
						<input type="text" className="small spacer" value={obj.modifier} onChange={(e) => this.props.changeHandler(this.props.contract, 'modifier', e.target.value, i)} />
						<textarea className="spacer" value={obj.situation} onChange={(e) => this.props.changeHandler(this.props.contract, 'situation', e.target.value, i)} />
					</div>)
				)
			})
		}
		else
		{
			out = {
				mods: [],
				sits: []
			}
			
			this.props.contract.modifiers.forEach((obj, i) => {
				out.mods.push(
					(<div key={`modifier-${i}`}>{obj.modifier}</div>)
				)
				out.sits.push(
					(<div key={`situation-${i}`}>{obj.situation}</div>)
				)
			})
		}
		
		return out
	}
	
	toggleEditMode()
	{
		let mode = this.state.editMode
		this.setState(() => { return { editMode: !mode } })
	}
}
